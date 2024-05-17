import { ButtonWrapper } from '@/components'
import DropdownSvg from '@/assets/dropdown.svg'
import { useNavigate } from 'react-router-dom'
import { FormEvent, useState } from 'react'

export default function AddSpiceMixPage() {
  const navigate = useNavigate()
  const [isFetchingData, setIsFetchingData] = useState(false)
  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()
  }
  
  return <div>
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
      <input id="name" name="name"/>
      
      <button type="submit" disabled={isFetchingData}>
        Add spice
      </button>
    </form>
    </div>
    }