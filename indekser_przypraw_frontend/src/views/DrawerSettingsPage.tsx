import { useNavigate, useParams } from 'react-router-dom'
import { FormEvent, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { StoreState } from '@/stores/store.ts'
import spiceApi from '@/wretchConfig.ts'
import Drawer from '@/types/Drawer.ts'
import FetchError from '@/components/FetchError.tsx'
import {
  editDrawer,
  removeSpiceGroupFromDrawer,
  updateSpiceGroupFromDrawer,
} from '@/stores/spiceStore.ts'
import './DrawerSettingsPage.css'
import { ApiError, SpiceGroup } from '@/types'
import { ButtonWrapper } from '@/components'
import DropdownSvg from '@/assets/dropdown.svg'

export default function DrawerSettingsPage() {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  const drawers = useSelector((state: StoreState) => state.spice.drawers)
  const { drawerId } = useParams()
  const drawer = drawers.find(
    (drawer) => drawer.drawerId === parseInt(drawerId!)
  )

  const [nameErrors, setNameErrors] = useState<{ [key: string]: string[] }>()

  const submitEditName = (ev: FormEvent<HTMLFormElement>) => {
    ev.preventDefault()
    const formData = new FormData(ev.currentTarget)
    if (formData.get('name') === drawer?.name) return
    spiceApi
      .put(Object.fromEntries(formData.entries()), 'Drawer/' + drawerId)
      .json<Drawer>()
      .then((json) => {
        dispatch(editDrawer({ drawerId: json.drawerId, name: json.name }))
      })
      .catch((err: Error) =>
        setNameErrors((JSON.parse(err.message) as ApiError).errors)
      )
  }

  const removeSpiceGroup = (spiceGroupId: number) => () => {
    if (window.confirm('Are you sure you want to delete this spice'))
      spiceApi
        .delete('Spices/group/' + spiceGroupId)
        .res(() => {
          if (!drawer) return
          dispatch(
            removeSpiceGroupFromDrawer({
              drawerId: drawer.drawerId,
              spiceGroupId,
            })
          )
        })
        .catch()
  }

  const changeMinimumsForSpiceGroup =
    (spiceGroupId: number) => (e: FormEvent<HTMLFormElement>) => {
      e.preventDefault()
      const json: Record<string, File | string | null> = Object.fromEntries(
        new FormData(e.currentTarget)
      )
      for (const key of Object.keys(json)) {
        json[key] = json[key] === '' ? null : json[key]
      }
      spiceApi
        .put(json, 'Spices/group/' + spiceGroupId)
        .json<SpiceGroup>()
        .then((spiceGroup) => {
          if (!drawer) return
          dispatch(
            updateSpiceGroupFromDrawer({
              drawerId: drawer?.drawerId,
              spiceGroup,
            })
          )
        })
    }
  return (
    <div id="drawer-settings">
      <div className="header">
        <ButtonWrapper
          onClick={() => navigate(-1)}
          additionalClasses={['full-height-button', 'header-item-height']}
        >
          <img className="go-back-img" src={DropdownSvg} alt="go back button" />
        </ButtonWrapper>
        <h1 className="jetbrains-mono-normal no-text-wrap max-width header-title header-item-height">
          {drawer?.name}
        </h1>
      </div>
      <div id="settings">
        <div id="edit-name">
          <h2 className="montserrat-bold">Name</h2>
          <form onSubmit={submitEditName}>
            <input id="name" name="name" defaultValue={drawer?.name} />
            <button key="name-submit" type="submit">
              Edit
            </button>
          </form>

          {nameErrors && <FetchError errors={nameErrors} />}
        </div>
        <div className="spice-settings">
          <h1 className="montserrat-bold">Spices</h1>
          {drawer &&
            drawer.spices.map((spice) => (
              <div key={spice.spiceGroupId}>
                <h2 className="montserrat-bold">{spice.name}</h2>
                <form
                  id={spice.spiceGroupId.toString()}
                  className="form-row"
                  onSubmit={changeMinimumsForSpiceGroup(spice.spiceGroupId)}
                >
                  <span>Min grams: </span>
                  <input
                    defaultValue={spice?.minimumGrams ?? ''}
                    name="minimumGrams"
                    id="minimumGrams"
                    type="number"
                  />
                  <span>Min packages: </span>
                  <input
                    defaultValue={spice?.minimumCount ?? ''}
                    name="minimumCount"
                    id="minimumCount"
                    type="number"
                  />
                </form>
                <button form={spice.spiceGroupId.toString()} type="submit">
                  Accept changes
                </button>
                <button onClick={removeSpiceGroup(spice.spiceGroupId)}>
                  Delete
                </button>
              </div>
            ))}
        </div>
      </div>
    </div>
  )
}
