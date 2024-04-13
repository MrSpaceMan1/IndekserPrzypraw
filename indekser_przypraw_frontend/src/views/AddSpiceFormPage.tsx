import { useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import { FormEvent, useState } from 'react'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'

export default function AddSpiceFormPage() {
  const barcodeInfo = useSelector(
    (state: StoreState) => state.barcode.fetchedBarcodeInfo
  )
  
  // useEffectOnce(() => {
  //   window.alert(JSON.stringify(barcodeInfo, null,  2))
  // })
  const [name, setName] = useState<string>(barcodeInfo.name)
  const [barcode, setBarcode] = useState<string>(barcodeInfo.barcode)
  const [grams, setGrams] = useState<number>(barcodeInfo.grams)

  function handleSubmit(ev: FormEvent<HTMLFormElement>) {
    ev.preventDefault()
    const formData = new FormData(ev.target as HTMLFormElement)
    const dataJson = Object.fromEntries(formData)
    console.log(dataJson)
  }

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <label htmlFor="Barcode">Barcode</label>
        <input id="Barcode" name="Barcode" value={barcode} readOnly={true} />
        <label htmlFor="Name">Name</label>
        <input
          id="Name"
          name="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          minLength={3}
        />
        <label htmlFor="Grams">Grams</label>
        <input
          id="Grams"
          name="Grams"
          type="number"
          value={grams}
          onChange={(e) => setGrams(parseInt(e.target.value))}
          min={1}
        />
        <button type="submit">Add spice</button>
      </form>
    </div>
  )
}
