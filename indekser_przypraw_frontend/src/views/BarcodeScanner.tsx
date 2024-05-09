import Quagga, { QuaggaJSResultObject } from '@ericblade/quagga2'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import './MainPage.css'
import './BarcodeScanner.css'
import { useEffect, useRef, useState } from 'react'
import spiceApi from '@/wretchConfig.ts'
import BarcodeInfo from '@/types/BarcodeInfo.ts'
import {
  clearFetchedBarcodeInfo,
  setFetchedBarcodeInfo,
} from '@/stores/barcodeScannerStore.ts'
import { useNavigate } from 'react-router-dom'
import { useDispatch } from 'react-redux'
import { checkEan13Validity, checkEan8Validity } from '@/eanValidators.ts'

const codeToValidator: { [key: string]: (arg0: string) => boolean } = {
  ean_8: checkEan8Validity,
  ean_13: checkEan13Validity,
}

export default function BarcodeScanner() {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  const outputRef = useRef<HTMLDivElement>(null)
  const [deviceId, setDeviceId] = useState<string>('')
  const [devices, setDevices] = useState<MediaDeviceInfo[]>([])
  const isFetchingBarcodeInfo = useRef(false)

  function handleProcessed(result: QuaggaJSResultObject) {
    const drawingCtx = Quagga.canvas.ctx.overlay
    const drawingCanvas = Quagga.canvas.dom.overlay
    drawingCtx.font = '24px Arial'
    drawingCtx.fillStyle = 'green'

    if (result) {
      // console.warn('* quagga onProcessed', result);
      if (result.boxes) {
        drawingCtx.clearRect(
          0,
          0,
          parseInt(drawingCanvas.getAttribute('width')!),
          parseInt(drawingCanvas.getAttribute('height')!)
        )
        result.boxes
          .filter((box) => box !== result.box)
          .forEach((box) => {
            Quagga.ImageDebug.drawPath(box, { x: 0, y: 1 }, drawingCtx, {
              color: 'purple',
              lineWidth: 2,
            })
          })
      }
      if (result.box) {
        Quagga.ImageDebug.drawPath(result.box, { x: 0, y: 1 }, drawingCtx, {
          color: 'blue',
          lineWidth: 2,
        })
      }
    }
  }

  function getMedian(arr: number[]) {
    const newArr = [...arr] // copy the array before sorting, otherwise it mutates the array passed in, which is generally undesireable
    newArr.sort((a, b) => a - b)
    const half = Math.floor(newArr.length / 2)
    if (newArr.length % 2 === 1) {
      return newArr[half]
    }
    return (newArr[half - 1] + newArr[half]) / 2
  }

  function getMedianOfCodeErrors(
    decodedCodes: { error?: number; code: number; start: number; end: number }[]
  ) {
    const errors = decodedCodes
      .filter((x) => !!x.error)
      .flatMap((x) => x.error!)
    return getMedian(errors)
  }

  async function handleDetected(result: QuaggaJSResultObject) {
    console.log(result.codeResult.format)
    if (isFetchingBarcodeInfo.current) return
    const err = getMedianOfCodeErrors(result.codeResult.decodedCodes)

    if (
      err < 0.25 &&
      codeToValidator[result.codeResult.format](result.codeResult.code!)
    ) {
      isFetchingBarcodeInfo.current = true
      spiceApi
        .get(`Spices/barcode/${result.codeResult.code}`)
        .notFound(() => {
          dispatch(
            setFetchedBarcodeInfo({
              barcode: result.codeResult.code!,
              grams: 0,
              name: '',
            })
          )
        })
        .json<BarcodeInfo>((barcode) => {
          dispatch(setFetchedBarcodeInfo(barcode))
          return barcode
        })
        .finally(() => {
          isFetchingBarcodeInfo.current = false
          navigate('/add-spice')
          Quagga.stop()
        })
    }
  }

  useEffectOnce(() => {
    dispatch(clearFetchedBarcodeInfo())
    Quagga.CameraAccess.request(null, {
      aspectRatio: { min: 1, max: 2, ideal: 1.77 },
      facingMode: 'enviroment',
      height: { min: 720 },
      frameRate: { min: 30, ideal: 60 },
    }).then(() => {
      navigator.mediaDevices
        .enumerateDevices()
        .then((devices): MediaDeviceInfo[] => {
          return devices.map((device) => device.toJSON() as MediaDeviceInfo)
        })
        .then((devices) => {
          const videoDevices = devices.filter((d) => d.kind === 'videoinput')
          setDevices(videoDevices)
          setDeviceId(videoDevices?.[0].deviceId)
        })
    })
  })

  useEffect(() => {
    if (!deviceId) return
    Quagga.init({
      inputStream: {
        type: 'LiveStream',
        willReadFrequently: true,
        target: outputRef.current ?? undefined,
        constraints: {
          aspectRatio: { min: 1, max: 2, ideal: 1.77 },
          ...(deviceId && { deviceId: deviceId }),
          facingMode: 'enviroment',
          height: { min: 720 },
          frameRate: { min: 30 },
        },
      },
      locate: true,
      locator: {
        halfSample: true,
        patchSize: 'large',
      },
      frequency: 15,
      decoder: {
        readers: ['ean_reader', 'ean_8_reader'],
      },
    })
      .then(() => {
        Quagga.onDetected((data) => {
          handleDetected(data)
        })
        Quagga.onProcessed(handleProcessed)
        Quagga.start()
      })
      .catch((err) => console.log(err))

    return () => {
      Quagga.stop()
      Quagga.offProcessed()
      Quagga.offDetected()
    }
  }, [deviceId])

  return (
    <div id="barcode-scanner">
      <select
        onChange={(e) => {
          setDeviceId(e.target.selectedOptions.item(0)?.value ?? '')
        }}
        value={deviceId}
      >
        {devices.map((device) => (
          <option key={device.deviceId} value={device.deviceId}>
            {device.label}
          </option>
        ))}
      </select>
      <div ref={outputRef} className="cameraFeed"></div>
    </div>
  )
}
