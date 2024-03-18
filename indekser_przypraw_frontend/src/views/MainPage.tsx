import { useRef, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { ButtonWrapper, NestedList, SideMenu } from '@/components'
import { StoreState } from '@/stores/store.ts'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import { useTouch } from '@/components/TouchRegionContext.tsx'
import hamburgerMenuSvg from '@/assets/hamburger_menu.svg'
import gearSvg from '@/assets/gear.svg'
import addNewSvg from '@/assets/add_new.svg'
import './MainPage.css'
import { setDebugMessage } from '@/stores/debugStore.ts'
import { useNavigate } from 'react-router-dom'

export default function MainPage() {
  const touch = useTouch()
  const [sideMenuOpen, setSideMenuOpen] = useState(false)
  const isSideMenuOpen = useRef(false)
  const sideMenuBackdropRef = useRef<HTMLDivElement>(null)
  const debugMessage = useSelector(
    (state: StoreState) => state.debug.debugMessage
  )
  const dispatch = useDispatch()
  const navigate = useNavigate()
  function openSideMenu() {
    setSideMenuOpen(true)
    isSideMenuOpen.current = true
  }

  function closeSideMenu() {
    setSideMenuOpen(false)
    isSideMenuOpen.current = false
  }

  const swipeHandler = (ev: HammerInput) => {
    dispatch(setDebugMessage(ev.direction === 4 ? 'right-swipe' : 'left-swipe'))
    if (ev.direction === 4) return openSideMenu()
    if (ev.direction === 2 && isSideMenuOpen.current) {
      return closeSideMenu()
    }
    return navigate('/barcode-scanner')
  }

  useEffectOnce(() => {
    touch.addListener('swipe', swipeHandler)
    return () => {
      touch.removeListener('swipe', swipeHandler)
    }
  })

  return (
    <div id="main-page">
      <SideMenu
        isOpen={sideMenuOpen}
        closeMenu={closeSideMenu}
        backdropRef={sideMenuBackdropRef}
      ></SideMenu>
      <header className="header">
        <ButtonWrapper onClick={openSideMenu}>
          <img src={hamburgerMenuSvg} className="header-icon" />
        </ButtonWrapper>
        <h1 className="jetbrains-mono-normal no-text-wrap max-width header-title ">
          DRAWER
        </h1>
        <ButtonWrapper onClick={() => {}}>
          <img src={gearSvg} className="header-icon black-icon-filter" />
        </ButtonWrapper>
      </header>
      {debugMessage}
      <div id="main-content">
        <NestedList listClassNames="no-list-styling">
          <NestedList
            label={<h2>TestOWE</h2>}
            listClassNames={'no-list-styling'}
          >
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
            <p>ABC </p>
          </NestedList>
        </NestedList>
      </div>
      <div id="action-bar">
        <ButtonWrapper onClick={() => {}}>
          <img src={addNewSvg} />
        </ButtonWrapper>
      </div>
    </div>
  )
}
