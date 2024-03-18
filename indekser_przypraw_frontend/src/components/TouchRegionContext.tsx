import { createContext, ReactElement, useContext } from 'react'
import { TouchRegion } from '@/components/index.ts'

enum HammerJSEvents {
  'swipe',
  'pan',
}
interface defaultTouchContextType {
  listeners: { [key in keyof typeof HammerJSEvents]?: Array<HammerListener> }
  addListener(
    evName: keyof typeof HammerJSEvents,
    evHandler: HammerListener
  ): void
  removeListener(
    evName: keyof typeof HammerJSEvents,
    evHandler: HammerListener
  ): void
}

export const defaultTouchContext: defaultTouchContextType = {
  listeners: {},
  addListener(evName, evHandler) {
    if (!this.listeners[evName]) {
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      // @ts-ignore
      this.listeners[evName] = []
    }
    this.listeners[evName]?.unshift(evHandler)
  },
  removeListener(evName, evHandler) {
    const index = this.listeners[evName]?.findIndex(
      (listener) => listener === evHandler
    )
    if (!index) return
    this.listeners[evName]?.splice(index, 1)
  },
}
const TouchContext = createContext(defaultTouchContext)

export function TouchContextProvider({ children }: TouchContextProviderProps) {
  return (
    <TouchContext.Provider value={defaultTouchContext}>
      <TouchRegion>{children}</TouchRegion>
    </TouchContext.Provider>
  )
}

interface TouchContextProviderProps {
  children?: ReactElement | ReactElement[]
}

export function useTouch() {
  return useContext(TouchContext)
}
