import { useRef, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { ButtonWrapper, NestedList, SideMenu } from '@/components'
import { StoreState } from '@/stores/store.ts'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import { useTouch } from '@/components/TouchRegionContext.tsx'
import hamburgerMenuSvg from '@/assets/hamburger_menu.svg'
import gearSvg from '@/assets/gear.svg'
import addNewSvg from '@/assets/add_new.svg'
import crossSvg from '@/assets/cross.svg'
import './MainPage.css'
import { setDebugMessage } from '@/stores/debugStore.ts'
import { useNavigate } from 'react-router-dom'
import '../components/NestedListItem.css'
import '../components/NestedList.css'
import { addSpiceToDrawer } from '@/stores/SpiceStore.ts'

export default function MainPage() {
  const touch = useTouch()
  const dispatch = useDispatch()
  const navigate = useNavigate()

  const drawers = useSelector((store: StoreState) => store.spice.drawers)

  const [drawerIndex, setDrawerIndex] = useState<number>(0)
  const [sideMenuOpen, setSideMenuOpen] = useState(false)
  const drawer = drawers[drawerIndex]

  const isSideMenuOpen = useRef(false)
  const sideMenuBackdropRef = useRef<HTMLDivElement>(null)

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

  if (!drawers) return <div>Loading</div>
  return (
    <div id="main-page">
      <SideMenu
        isOpen={sideMenuOpen}
        closeMenu={closeSideMenu}
        backdropRef={sideMenuBackdropRef}
      >
        {drawers.map((drawer, index) => (
          <button
            className="menu-item"
            onClick={() => {
              setDrawerIndex(index)
              closeSideMenu()
            }}
          >
            {drawer.name}
          </button>
        ))}
      </SideMenu>

      <header className="header">
        <ButtonWrapper onClick={openSideMenu}>
          <img src={hamburgerMenuSvg} className="header-icon" />
        </ButtonWrapper>
        <h1 className="jetbrains-mono-normal no-text-wrap max-width header-title ">
          {drawer?.name ?? 'Drawer'}
        </h1>
        <ButtonWrapper onClick={() => {}}>
          <img src={gearSvg} className="header-icon black-icon-filter" />
        </ButtonWrapper>
      </header>
      <div id="main-content">
        {drawer &&
          drawer.spices.map((spiceGroup) => (
            <NestedList label={<h2>{spiceGroup.name}</h2>}>
              <table className={'nested-list-body'}>
                <thead>
                  <tr>
                    <th>#</th>
                    <th>Data ważności</th>
                    <th>Waga</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  {spiceGroup.spices.map((spice, index) => (
                    <tr key={spice.spiceId}>
                      <td>{index + 1}</td>
                      <td>{spice.expirationDate}</td>
                      <td>{spice.grams}g</td>
                      <td>
                        <button className={'unset'}>
                          <img src={crossSvg} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </NestedList>
          ))}
      </div>
      <div id="action-bar">
        <ButtonWrapper onClick={() => {}}>
          <img src={addNewSvg} />
        </ButtonWrapper>
      </div>
    </div>
  )
}
