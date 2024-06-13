import { useDispatch, useSelector } from 'react-redux'
import { FormEvent, useState } from 'react'
import spiceApi from '@/wretchConfig.ts'
import { useNavigate } from 'react-router-dom'
import { addDrawer, setSelected } from '@/stores/spiceStore.ts'
import { ApiError, Drawer } from '@/types'
import FetchError from '@/components/FetchError.tsx'
import './AddStuffFormPage.css'
import DropdownSvg from '@/assets/dropdown.svg'
import { ButtonWrapper } from '@/components'
import { StoreState } from '@/stores/store.ts'

export default function AddDrawerFormPage() {
  const navigate = useNavigate()
  const dispatch = useDispatch()
  const drawers = useSelector((state: StoreState) => state.spice.drawers)
  const [name, setName] = useState<string>('')
  const [errors, setErrors] = useState<{ [key: string]: string[] }>({})
  const [isFetchingData, setIsFetchingData] = useState<boolean>(false)

  async function handleSubmit(ev: FormEvent<HTMLFormElement>) {
    setIsFetchingData(true)
    ev.preventDefault()
    const formData = new FormData(ev.target as HTMLFormElement)
    const dataJson = Object.fromEntries(formData)

    spiceApi
      .post(dataJson, 'Drawer')
      .json<Drawer>()
      .then((drawer) => {
        dispatch(addDrawer(drawer))
        dispatch(setSelected(drawers.length))
        navigate('/', { relative: 'route' })
      })
      .catch((err: Error) => JSON.parse(err.message))
      .then((err: ApiError) => {
        setErrors(err.errors ?? {})
      })
      .finally(() => setIsFetchingData(false))
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
        <label htmlFor="name">Drawer name</label>
        <input
          id="name"
          name="name"
          value={name}
          required={true}
          onChange={(e) => setName(e.target.value)}
        />
        <button type="submit" disabled={isFetchingData}>
          Add drawer
        </button>
      </form>
      <FetchError errors={errors}></FetchError>
    </div>
  )
}
