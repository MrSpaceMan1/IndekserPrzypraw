import { ReactElement, useRef } from 'react'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import Hammer from 'hammerjs'
import { useTouch } from '@/components/TouchRegionContext.tsx'

export default function TouchRegion({ children }: TouchRegionProps) {
  const touchRef = useRef<HTMLDivElement>(null)
  const touchManager = useRef<HammerManager>()
  const touch = useTouch()
  useEffectOnce(() => {
    touchManager.current = new Hammer(touchRef.current!)
    touchManager.current?.on('pan', (ev: HammerInput) => {
      touch.listeners.pan?.forEach((eventHandler) => eventHandler(ev))
    })
    touchManager.current?.on('swipe', (ev: HammerInput) => {
      touch.listeners.swipe?.forEach((eventHandler) => eventHandler(ev))
    })
  })
  return (
    <div style={{ height: '100%', width: '100%' }} ref={touchRef}>
      {children}
    </div>
  )
}

interface TouchRegionProps {
  children?: ReactElement | ReactElement[]
}
