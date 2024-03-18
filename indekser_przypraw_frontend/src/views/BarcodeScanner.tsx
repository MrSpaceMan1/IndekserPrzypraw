import Quagga, { QuaggaJSResultObject } from '@ericblade/quagga2'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import './MainPage.css'
import './BarcodeScanner.css'
import { ChangeEvent, useRef, useState } from 'react'
import Scanner from '@/components/Scanner.tsx'

export default function BarcodeScanner() {
  const [cameraError, setCameraError] = useState<string>()
  const [cameras, setCameras] = useState<MediaDeviceInfo[]>([])
  const videoRef = useRef<HTMLVideoElement>(null)
  const mediaStream = useRef<MediaStream>(null)

  async function getVideoStream(ev: ChangeEvent<HTMLSelectElement>) {
    mediaStream.current?.getTracks().forEach((track) => track.stop())
    navigator.mediaDevices
      .getUserMedia({
        video: {
          deviceId: ev.target.selectedOptions?.[0].value,
        },
      })
      .then((currentMediaStream) => {
        videoRef.current.srcObject = currentMediaStream
        mediaStream.current = currentMediaStream
      })
  }

  useEffectOnce(() => {
    const enableCamera = async () => {
      await Quagga.CameraAccess.request(null, {})
    }
    const disableCamera = async () => {
      await Quagga.CameraAccess.release()
    }
    const enumerateCameras = async () => {
      const cameras = await Quagga.CameraAccess.enumerateVideoDevices()
      console.log('Cameras Detected: ', cameras)
      setCameras(cameras)
      return cameras
    }

    navigator.mediaDevices
      .getUserMedia({ video: true, audio: false })
      .then((stream) => {
        navigator.mediaDevices
          .enumerateDevices()
          .then((devices) =>
            devices.filter((value) => value.kind == 'videoinput')
          )
          .then(setCameras)
        return stream
      })

    // enableCamera()
    //   .then(disableCamera)
    //   .then(enumerateCameras)
    //   .catch((err) => setCameraError(err))
    return () => disableCamera()
  })

  return (
    <div id="barcode-scanner" className="">
      {cameraError && cameraError}
      {cameras && JSON.stringify(cameras)}
      <select onChange={getVideoStream}>
        {cameras.length &&
          cameras.map((camera) => (
            <option value={camera.deviceId}>{camera.label}</option>
          ))}
      </select>
      <video autoPlay={true} ref={videoRef}></video>
    </div>
  )
}
