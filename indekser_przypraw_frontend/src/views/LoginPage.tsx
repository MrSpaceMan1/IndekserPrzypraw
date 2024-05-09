import { FormEvent, useState } from 'react'
import './LoginPage.css'
import spiceApi from '@/wretchConfig.ts'
import { useDispatch } from 'react-redux'
import { Drawer } from '@/types'
import { setDrawers } from '@/stores/spiceStore.ts'
import { useNavigate } from 'react-router-dom'

export default function LoginPage() {
  const [loginMode, setLoginMode] = useState(true)
  const dispatch = useDispatch()
  const navigate = useNavigate()

  const handleLogin = (ev: FormEvent<HTMLFormElement>) => {
    ev.preventDefault()
    const formData = new FormData(ev.target as HTMLFormElement)
    const data = Object.fromEntries(formData.entries())
    spiceApi
      .url('login')
      .url('?useCookies=true&useSessionCookies=true')
      .json(data)
      .post()
      .res()
      .then(() => spiceApi.url('Drawer').get().json<Array<Drawer>>())
      .then((drawers) => dispatch(setDrawers(drawers)))
      .then(() => navigate('/', { replace: true }))
      .catch()
  }

  const handleRegister = (ev: FormEvent<HTMLFormElement>) => {
    ev.preventDefault()
    const formData = new FormData(ev.target as HTMLFormElement)
    const data = Object.fromEntries(formData.entries())
    console.log(data)
  }

  return (
    <div id="login-container">
      <div className="login-header">
        <h1 className="jetbrains-mono-normal no-text-wrap max-width header-title">
          {loginMode ? 'Log in' : 'Register'}
        </h1>
      </div>
      <form id="login-form" onSubmit={loginMode ? handleLogin : handleRegister}>
        <label htmlFor="email">Email</label>
        <input name="email" id="email" type="email" />
        <label htmlFor="password">Password</label>
        <input name="password" id="password" type="password" />
        <button type="submit">{loginMode ? 'Log in' : 'Register'}</button>
      </form>
      <button
        className="unset underline"
        onClick={() => setLoginMode(!loginMode)}
      >
        {loginMode ? 'Register?' : 'Login?'}
      </button>
    </div>
  )
}
