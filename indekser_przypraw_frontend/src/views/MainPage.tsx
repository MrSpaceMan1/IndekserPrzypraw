import { useRef, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { ButtonWrapper, SideMenu, SpiceList } from '@/components'
import { StoreState } from '@/stores/store.ts'
import hamburgerMenuSvg from '@/assets/hamburger_menu.svg'
import gearSvg from '@/assets/gear.svg'
import addNewSvg from '@/assets/add_new.svg'
import funnelSvg from '@/assets/funnel.svg'
import './MainPage.css'
import { useNavigate } from 'react-router-dom'
import '../components/NestedListItem.css'
import '../components/NestedList.css'
import { Tooltip } from 'react-tooltip'
import { setSelected } from '@/stores/spiceStore.ts'
import filteredList from '@/components/SpiceListFiltered.ts'
import shoppingListSvf from '@/assets/shopping-list.svg'

export default function MainPage() {
  const dispatch = useDispatch()
  const navigate = useNavigate()

  const drawers = useSelector((store: StoreState) => store.spice.drawers)
  const selected = useSelector(
    (store: StoreState) => store.spice.selectedDrawer
  )

  const [filterString, setFilterString] = useState<string>('')
  const [sideMenuOpen, setSideMenuOpen] = useState(false)
  const drawer = drawers[selected]

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

  if (!drawers) return <div>Loading</div>
  return (
    <div id="main-page">
      <SideMenu
        isOpen={sideMenuOpen}
        closeMenu={closeSideMenu}
        backdropRef={sideMenuBackdropRef}
      >
        {drawers?.map((drawer, index) => (
          <button
            key={drawer.drawerId}
            className="menu-item unset"
            onClick={() => {
              dispatch(setSelected(index))
              closeSideMenu()
            }}
          >
            {drawer.name}
          </button>
        ))}
        <button
          className="menu-item unset"
          onClick={() => navigate('/add-drawer')}
        >
          +
        </button>
      </SideMenu>

      <header className="header">
        <ButtonWrapper
          onClick={openSideMenu}
          additionalClasses={['header-item-height']}
        >
          <img
            src={hamburgerMenuSvg}
            className="header-icon"
            alt="Show drawers icon"
          />
        </ButtonWrapper>
        <Tooltip id="drawer-name" />
        <ButtonWrapper
          onClick={() => navigate('/drawer/' + drawer.drawerId + '/settings')}
          additionalClasses={['header-item-height']}
        >
          <h1
            className="jetbrains-mono-normal no-text-wrap max-width header-title underline"
            data-tooltip-id="drawer-name"
            data-tooltip-content={drawer?.name}
            data-tooltip-place="top"
          >
            {drawer?.name ?? 'Drawer'}
          </h1>
        </ButtonWrapper>
        <ButtonWrapper
          onClick={() => {}}
          additionalClasses={['header-item-height']}
        >
          <img
            src={gearSvg}
            className="header-icon black-icon-filter"
            alt="Drawer settings icon"
          />
        </ButtonWrapper>
      </header>
      <div>
        <span className="filter-bar-row">
          <img src={funnelSvg} className="filter-bar-icon" alt="Filter icon" />
          <input
            type="text"
            value={filterString}
            onChange={(e) => setFilterString(e.target.value)}
            placeholder="filter"
          />
        </span>
      </div>
      <div id="main-content">
        {drawer &&
          (
            (filterString
              ? filteredList(drawer.spices, (item) => item.name, filterString)
              : drawer.spices) ?? []
          ).map((spiceGroup) => (
            <SpiceList key={spiceGroup.spiceGroupId} spiceGroup={spiceGroup} />
          ))}
      </div>
      <div id="action-bar">
        <ButtonWrapper
          onClick={() => {
            navigate('/shopping-list')
          }}
        >
          <img
            src={shoppingListSvf}
            className="header-icon black-icon-filter"
            alt="view shopping list"
          />
        </ButtonWrapper>
        <ButtonWrapper onClick={() => navigate('/barcode-scanner')}>
          <img src={addNewSvg} alt="Add new" />
        </ButtonWrapper>
        <ButtonWrapper onClick={() => navigate('/spice-mixes')}>
          <img src={addNewSvg} alt="Add new" />
        </ButtonWrapper>
      </div>
    </div>
  )
}
