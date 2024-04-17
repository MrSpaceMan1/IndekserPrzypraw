import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import spiceApi from '@/wretchConfig.ts'
import Drawer from '@/types/Drawer.ts'
import { useDispatch } from 'react-redux'
import { setDrawers } from '@/stores/SpiceStore.ts'
import { ReactNode } from 'react'

export function App({ children }: { children: ReactNode }) {
  const dispatch = useDispatch()
  useEffectOnce(() => {
    spiceApi
      .get('Drawer')
      .json<Array<Drawer>>()
      .then((drawers) => dispatch(setDrawers(drawers)))
      .catch((error) => console.log(error))
  })

  return <>{children}</>
}
