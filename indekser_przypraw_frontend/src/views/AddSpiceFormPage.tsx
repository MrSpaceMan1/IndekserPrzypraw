import { useDispatch, useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import { FormEvent, useState } from 'react'
import spiceApi from '@/wretchConfig.ts'
import { useNavigate } from 'react-router-dom'
import { addSpiceToDrawer } from '@/stores/spiceStore.ts'
import { Spice } from '@/types'
import { ButtonWrapper } from '@/components'
import DropdownSvg from '@/assets/dropdown.svg'
import './AddStuffFormPage.css'

export default function AddSpiceFormPage() {
  const navigate = useNavigate()
  const barcodeInfo = useSelector(
    (state: StoreState) => state.barcode.fetchedBarcodeInfo
  )

  const drawers = useSelector((state: StoreState) => state.spice.drawers)
  const dispatch = useDispatch()
  const [name, setName] = useState<string>(barcodeInfo.name)
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [barcode, _] = useState<string>(barcodeInfo.barcode)
  const [grams, setGrams] = useState<number>(barcodeInfo.grams)
  const [expirationDate, setExpirationDate] = useState<string>('')
  const [drawer, setDrawer] = useState<number>(drawers?.[0]?.drawerId)
  const [isFetchingData, setIsFetchingData] = useState<boolean>(false)

  async function handleSubmit(ev: FormEvent<HTMLFormElement>) {
    console.log(ev)
    setIsFetchingData(true)
    ev.preventDefault()
    const formData = new FormData(ev.target as HTMLFormElement)
    const dataJson = Object.fromEntries(
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      Array.from(formData).filter(([_, value]) => !!value)
    )
    spiceApi
      .post(dataJson, 'Spices/drawer/' + drawer)
      .json<Spice>()
      .then((spice) => {
        dispatch(addSpiceToDrawer({ drawerId: drawer, spice }))
        navigate('/', { relative: 'route' })
      })
      .catch((err) => {
        console.log(err)
        spiceApi
          .get()
          .res()
          .then(() => navigate('/login'))
      })
      .finally(() => setIsFetchingData(false))
    console.log(dataJson, 'Spices/drawer/' + drawer)
  }

  return (
    <div>
      <div className="header">
        <ButtonWrapper
          onClick={() => navigate(-1)}
          additionalClasses={['full-height-button', 'header-item-height']}
        >
          <img className="go-back-img" src={DropdownSvg} alt="go back button" />
        </ButtonWrapper>
        <h1 className="full-width header-title jetbrains-mono-normal">
          ADD DRAWER
        </h1>
      </div>
      <form className="addStuffForm" onSubmit={handleSubmit}>
        <label htmlFor="barcode">Barcode</label>
        <input
          id="barcode"
          name="barcode"
          defaultValue={barcode}
          required={true}
          readOnly={!!barcodeInfo.barcode}
        />
        <label htmlFor="name">Name</label>
        <input
          id="name"
          name="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          minLength={3}
          required={true}
          readOnly={!!barcodeInfo.name}
        />
        <label htmlFor="Grams">Grams</label>
        <input
          id="grams"
          name="grams"
          type="number"
          value={grams}
          onChange={(e) => setGrams(parseInt(e.target.value))}
          min={1}
          required={true}
          readOnly={!!barcodeInfo.grams}
        />
        <label htmlFor="expirationDate">Expiration Date</label>
        <input
          id="expirationDate"
          name="expirationDate"
          value={expirationDate}
          type="date"
          onChange={(e) => setExpirationDate(e.target.value)}
        />
        <label htmlFor="drawer">Drawer</label>
        <select
          id="drawer"
          name="drawer"
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
