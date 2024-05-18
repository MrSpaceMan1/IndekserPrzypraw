import { ButtonWrapper } from '@/components'
import DropdownSvg from '@/assets/dropdown.svg'
import { useNavigate } from 'react-router-dom'
import { FormEvent, useState } from 'react'
import { useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import SpiceGroup from '@/types/SpiceGroup.ts'

export default function AddSpiceMixFormPage() {
  const navigate = useNavigate()
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
    // @ts-ignore
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
        <input name="name" id="name" />
        <label htmlFor="ingredients" />
        {ingredients.map((_, index) => (
          <div key={'ingredient-' + index}>
            <select
              name={'ingredients-' + index + '-name'}
              id={'ingredients-' + index + '-name'}
            >
              {spiceGroups &&
                spiceGroups.map((sg) => (
                  <option value={sg.name}>{sg.name}</option>
                ))}
            </select>
            <input
              id={'ingredients-' + index + '-grams'}
              name={'ingredients-' + index + '-grams'}
              className="shortInput"
              type="number"
              defaultValue={0}
              min="0"
              max="99"
            />
            g
          </div>
        ))}
        <button
          type="button"
          onClick={() => setIngredients([0, ...ingredients])}
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
