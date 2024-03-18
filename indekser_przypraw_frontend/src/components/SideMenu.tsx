import './SideMenu.css'
import { join } from '@/utils.ts'
import { ReactElement, Ref } from 'react'
export default function SideMenu({
  isOpen,
  closeMenu,
  children,
  backdropRef,
}: SideMenuProps) {
  const isOpenBackdropClass = isOpen ? 'backdrop-open' : ''
  const isOpenMenuClass = isOpen ? 'menu-open' : 'menu-close'
  return (
    <>
      <div
        ref={backdropRef}
        className={join(' ', 'backdrop', isOpenBackdropClass)}
        onClick={(e) => {
          if (!(e.target as HTMLDivElement).classList.contains('backdrop'))
            return
          closeMenu()
        }}
      />
      <div className={join(' ', 'menu', isOpenMenuClass)}>{children}</div>
    </>
  )
}

interface SideMenuProps {
  isOpen: boolean
  backdropRef: Ref<HTMLDivElement>
  closeMenu: () => void
  children?: ReactElement | ReactElement[]
}
