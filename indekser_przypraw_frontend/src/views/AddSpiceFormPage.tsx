import { useDispatch, useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import { FormEvent, useEffect, useState } from 'react'
import spiceApi from '@/wretchConfig.ts'
import { useNavigate } from 'react-router-dom'
import { addSpiceToDrawer } from '@/stores/SpiceStore.ts'
import { Spice } from '@/types/Spice.ts'

export default function AddSpiceFormPage() {
  const navigate = useNavigate()
  const barcodeInfo = useSelector(
    (state: StoreState) => state.barcode.fetchedBarcodeInfo
  )

  const drawers = useSelector((state: StoreState) => state.spice.drawers)
  const dispatch = useDispatch()
  const [name, setName] = useState<string>(barcodeInfo.name)
  const [barcode, _] = useState<string>(barcodeInfo.barcode)
  const [grams, setGrams] = useState<number>(barcodeInfo.grams)
  const [expirationDate, setExpirationDate] = useState<string>('')
  const [drawer, setDrawer] = useState<number>(drawers?.[0]?.drawerId)
  const [isFetchingData, setIsFetchingData] = useState<boolean>(false)

  async function handleSubmit(ev: FormEvent<HTMLFormElement>) {
    setIsFetchingData(true)
    ev.preventDefault()
    const formData = new FormData(ev.target as HTMLFormElement)
    const dataJson = Object.fromEntries(formData)
    spiceApi
      .post(dataJson, 'Spices/drawer/' + drawer)
      .json<Spice>()
      .then((spice) => {
        dispatch(addSpiceToDrawer({ drawerId: drawer, spice }))
        navigate('/', { relative: 'route' })
      })
      .finally(() => setIsFetchingData(false))
    console.log(dataJson, 'Spices/drawer/' + drawer)
  }

  useEffect(() => {
    console.log(barcodeInfo)
  }, [barcodeInfo])

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <label htmlFor="Barcode">Barcode</label>
        <input
          id="Barcode"
          name="Barcode"
          value={barcode}
          required={true}
          readOnly={true}
        />
        <label htmlFor="Name">Name</label>
        <input
          id="Name"
          name="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          minLength={3}
          required={true}
          readOnly={!!barcodeInfo.name}
        />
        <label htmlFor="Grams">Grams</label>
        <input
          id="Grams"
          name="Grams"
          type="number"
          value={grams}
          onChange={(e) => setGrams(parseInt(e.target.value))}
          min={1}
          required={true}
          readOnly={!!barcodeInfo.grams}
        />
        <label htmlFor="ExpirationDate">Expiration Date</label>
        <input
          id="ExpirationDate"
          name="ExpirationDate"
          value={expirationDate}
          type="date"
          onChange={(e) => setExpirationDate(e.target.value)}
        />
        <label htmlFor="Drawer">Drawer</label>
        <select
          id="Drawer"
          name="Drawer"
          value={drawer}
          onChange={(e) => {
            setDrawer(parseInt(e.target.value?.[0]))
          }}
        >
          {drawers.map((item) => (
            <option key={item.name} value={item.drawerId}>
              {item.name}
            </option>
          ))}
        </select>
        <button type="submit" disabled={isFetchingData}>
          Add spice
        </button>
      </form>
    </div>
  )
}
