import './SideMenu.css'
import { join } from '@/utils.ts'
import { ReactNode, Ref } from 'react'
export default function SideMenu({
  isOpen,
  closeMenu,
  children,
  backdropRef,
}: SideMenuProps) {
  const isOpenMenuClass = isOpen ? 'menu-open' : 'menu-close'
  return (
    <>
      <div
        ref={backdropRef}
        className={join(' ', 'backdrop', isOpen ? 'backdrop-open' : undefined)}
        onClick={(e) => {
          if (!(e.target as HTMLDivElement).classList.contains('backdrop'))
            return
          isOpen = false
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
  children?: ReactNode | ReactNode[]
}
