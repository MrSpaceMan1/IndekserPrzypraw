import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import spiceApi from '@/wretchConfig.ts'
import Drawer from '@/types/Drawer.ts'
import { useDispatch } from 'react-redux'
import { setDrawers } from '@/stores/spiceStore.ts'
import { Outlet, useNavigate } from 'react-router-dom'

export function App() {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  useEffectOnce(() => {
    (async () => {
      let isAvailable = null
      let drawers: Array<Drawer> | null = null
      try {
        isAvailable = await spiceApi.get().res()
        drawers = await spiceApi.get('Drawer').json<Array<Drawer>>()
        dispatch(setDrawers(drawers))
      } catch (e) {
        if (isAvailable && !drawers) navigate('/login')
      }
    })()
  })

  return (
    <>
      <Outlet />
    </>
  )
}
