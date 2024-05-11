import { useEffectOnce } from '@/hooks/UseEffectOnce.ts'
import spiceApi from '@/wretchConfig.ts'
import { MissingSpiceGroups, MissingSpices } from '@/types'
import { useState } from 'react'
import './ShoppingListPage.css'
import { ButtonWrapper } from '@/components'
import dropdownSvg from '@/assets/dropdown.svg'
import { useNavigate } from 'react-router-dom'

export default function ShoppingListPage() {
  const [missingSpices, setMissingSpices] = useState<{
    [key: string]: MissingSpiceGroups[]
  }>({})
  const navigate = useNavigate()
  useEffectOnce(() => {
    spiceApi
      .url('Spices/missing')
      .get()
      .json<{ missingSpices: MissingSpices }>()
      .then((missingSpices) => setMissingSpices(missingSpices.missingSpices))
  })
  return (
    <div style={{ height: '100%' }}>
      <div className="shopping-list-header">
        <ButtonWrapper
          onClick={() => navigate(-1)}
          additionalClasses={['full-height-button', 'header-item-height']}
        >
          <img className={'go-back-img'} src={dropdownSvg} alt="go back icon" />
        </ButtonWrapper>
        <h1
          style={{ lineHeight: 1.05 }}
          className="jetbrains-mono-normal max-width header-title"
        >
          Shopping list
        </h1>
      </div>
      <div id="shopping-list-section">
        {Object.entries(missingSpices).map(([title, missingSpiceGroups]) => (
          <div key={title} className="drawerSection montserrat-normal">
            <h2 className={'drawerTitle'}>{title}</h2>
            {missingSpiceGroups.map(
              ({ spiceGroupId, missingCount, missingGrams, name }) => (
                <div key={spiceGroupId}>
                  <h3 className="spiceGroupTitle">{name}</h3>
                  {missingGrams ? (
                    <div className="missingRow">
                      <span>Grams needed: </span>
                      <span className="montserrat-bold">{missingGrams}g</span>
                    </div>
                  ) : null}
                  {missingCount ? (
                    <div className="missingRow">
                      <span>Packages needed: </span>
                      <span className="montserrat-bold">
                        {missingCount} psc.
                      </span>
                    </div>
                  ) : null}
                </div>
              )
            )}
          </div>
        ))}
      </div>
    </div>
  )
}
