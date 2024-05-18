import { ButtonWrapper } from '@/components'
import gearSvg from '@/assets/gear.svg'
import funnelSvg from '@/assets/funnel.svg'
import dropdownSvg from '@/assets/dropdown.svg'
import { useNavigate } from 'react-router-dom'
import { useState } from 'react'
import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import { useDispatch, useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import spiceApi from '@/wretchConfig.ts'
import { SpiceMix } from '@/types'
import { setSpiceMixes } from '@/stores/spiceMixStore.ts'
import shoppingListSvf from '@/assets/shopping-list.svg'
import addNewSvg from '@/assets/add_new.svg'

export default function SpiceMixPage() {
  const navigate = useNavigate()
  const dispatch = useDispatch
  const spiceMixes = useSelector(
    (state: StoreState) => state.spiceMix.spiceMixes
  )
  const [filterString, setFilterString] = useState('')

  useEffectOnce(() => {
    if (spiceMixes.length) return
    spiceApi
      .url('SpiceMix')
      .get()
      .json<SpiceMix[]>()
      .then((mixes) => dispatch(setSpiceMixes(mixes)))
  })

  return (
    <>
      <header className="header">
        <ButtonWrapper
          onClick={() => navigate(-1)}
          additionalClasses={['full-height-button', 'header-item-height']}
        >
          <img className={'go-back-img'} src={dropdownSvg} alt="go back icon" />
        </ButtonWrapper>

        <h1 className="jetbrains-mono-normal no-text-wrap max-width header-title">
          Spice Mixes
        </h1>

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
        {spiceMixes && spiceMixes.map((spiceMix) => JSON.stringify(spiceMix))}
        {/*{drawer &&*/}
        {/*  (*/}
        {/*    (filterString*/}
        {/*      ? filteredSpiceList(drawer.spices, filterString)*/}
        {/*      : drawer.spices) ?? []*/}
        {/*  ).map((spiceGroup) => (*/}
        {/*    <SpiceList key={spiceGroup.spiceGroupId} spiceGroup={spiceGroup} />*/}
        {/*  ))}*/}
      </div>
      <div id="action-bar">
        <span className="header-icon"></span>
        <ButtonWrapper onClick={() => navigate('/add-spice-mix')}>
          <img src={addNewSvg} alt="Add new" />
        </ButtonWrapper>
        <span className="header-icon"></span>
      </div>
    </>
  )
}
