import { useRef } from 'react'
import { useDispatch } from 'react-redux'
import { Drawer, Spice } from '@/types'
import spiceApi from '@/wretchConfig.ts'
import { removeSpiceFromDrawer } from '@/stores/spiceStore.ts'
import crossSvg from '@/assets/cross.svg'
import './NestedListItem.css'

export default function NestedListItem({
  drawer,
  spice,
  index,
}: NestedListItemProps) {
  const dispatch = useDispatch()
  const isDeleting = useRef(false)

  const removeSpice = async () => {
    if (!window.confirm('Jesteś pewnien? Ta akcja nie może być cofnięta'))
      return
    spiceApi
      .delete('Spices/' + spice.spiceId)
      .res()
      .then(() => {
        dispatch(
          removeSpiceFromDrawer({ drawerId: drawer.drawerId, spice: spice })
        )
      })
      .catch((error) => console.error(error))
  }

  return (
    <tr key={spice.spiceId}>
      <td>{index + 1}</td>
      <td>{spice.expirationDate}</td>
      <td>{spice.grams}g</td>
      <td>
        <button
          disabled={isDeleting.current}
          className={'unset'}
          onClick={removeSpice}
        >
          <img src={crossSvg} />
        </button>
      </td>
    </tr>
  )
}

interface NestedListItemProps {
  drawer: Drawer
  spice: Spice
  index: number
}
