import { useDispatch } from 'react-redux'
import { FormEvent, useState } from 'react'
import spiceApi from '@/wretchConfig.ts'
import { useNavigate } from 'react-router-dom'
import { addDrawer } from '@/stores/spiceStore.ts'
import { ApiError, Drawer } from '@/types'
import FetchError from '@/components/FetchError.tsx'

export default function AddDrawerFormPage() {
  const navigate = useNavigate()
  const dispatch = useDispatch()
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
        navigate('/', { relative: 'route' })
      })
      .catch((err: Error) => JSON.parse(err.message))
      .then((err: ApiError) => {
        setErrors(err.errors)
      })
      .finally(() => setIsFetchingData(false))
  }

  return (
    <div>
      <form onSubmit={handleSubmit}>
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
