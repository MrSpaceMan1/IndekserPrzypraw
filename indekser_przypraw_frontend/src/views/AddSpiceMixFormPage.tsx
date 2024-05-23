import { ButtonWrapper } from '@/components'
import DropdownSvg from '@/assets/dropdown.svg'
import { json, useNavigate } from 'react-router-dom'
import { FormEvent, useReducer, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import SpiceGroup from '@/types/SpiceGroup.ts'
import spiceApi from '@/wretchConfig.ts'
import { SpiceMix } from '@/types'
import { addSpiceMix } from '@/stores/spiceMixStore.ts'

export default function AddSpiceMixFormPage() {
  const navigate = useNavigate()
  const dispatch = useDispatch()
  const [isFetchingData, setIsFetchingData] = useState()
  const [ingredients, setIngredients] = useState<number[]>([])
  const spiceGroups = useSelector(
    (state: StoreState) => state.spice.drawers
  ).reduce((sg, drawer) => [...sg, ...drawer.spices], new Array<SpiceGroup>())

  const handleSubmit = (ev: FormEvent<HTMLFormElement>) => {
    ev.preventDefault()
    const formData = new FormData(ev.currentTarget)
    const formJson = Array.from(formData.entries()).groupBy(
      ([key, _]) => key.split('-')?.[0]
    )
    // @ts-expect-error TS2322
    formJson['ingredients'] = Array.from(
      Object.entries(
        formJson['ingredients'].groupBy(([key, value]) => key.split('-')[1])
      )
    )
      .map(([key, value]) => value)
      .map((properties) =>
        Object.fromEntries(
          properties.map(([key, value]) => [key.split('-')[2], value])
        )
      )
    // @ts-expect-error TS2322
    formJson['name'] = formJson['name'][0][1]
    console.log(formJson)
    spiceApi
      .url('SpiceMix')
      .post(formJson)
      .json<SpiceMix>()
      .then((sm) => {
        dispatch(addSpiceMix(sm))
        navigate(-1)
      })
      .catch()
  }
  return (
    <div>
      <div className="header">
        <ButtonWrapper
          onClick={() => navigate(-1)}
          additionalClasses={['full-height-button', 'header-item-height']}
        >
          <img className="go-back-img" src={DropdownSvg} alt="go back button" />
        </ButtonWrapper>
        <h1 className="full-width header-title jetbrains-mono-normal">
          ADD SPICE MIX
        </h1>
      </div>
      <form className="addStuffForm" onSubmit={handleSubmit}>
        <label htmlFor="name">Name</label>
        <input name="name" id="name" required={true} />
        <label htmlFor="ingredients" />
        <div id="ingredients">
          {ingredients.map((_, index) => {
            return (
              <div key={'ingredient-' + index}>
                <select
                  key={'ingredient-' + index + '-name'}
                  name={'ingredients-' + index + '-name'}
                  id={'ingredients-' + index + '-name'}
                  onChange={() => {}}
                >
                  {spiceGroups &&
                    Array.from(
                      Set.constructBy(spiceGroups, (sg) => sg.name)
                    ).map((sg) => <option value={sg.name}>{sg.name}</option>)}
                </select>
                <input
                  key={'ingredient-' + index + '-grams'}
                  id={'ingredients-' + index + '-grams'}
                  name={'ingredients-' + index + '-grams'}
                  className="shortInput"
                  type="number"
                  defaultValue={0}
                  min="0"
                  max="99"
                />
                g{' '}
                <button
                  onClick={() =>
                    setIngredients(ingredients.slice(0, length - 1))
                  }
                >
                  X
                </button>
              </div>
            )
          })}
        </div>
        <button
          type="button"
          onClick={() => {
            setIngredients([0, ...ingredients])
          }}
        >
          Dodaj składnik
        </button>
        <button type="submit" disabled={isFetchingData}>
          Add spice
        </button>
      </form>
    </div>
  )
}
