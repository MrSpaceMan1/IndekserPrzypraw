import { NestedList } from '@/components'
import { SpiceMix } from '@/types'
import NestedSpiceMixListItem from '@/components/NestedSpiceMixListItem.tsx'

export default function SpiceMixList({ spiceMix }: SpiceMixListProps) {
  return (
    <NestedList label={<h2>{spiceMix.name}</h2>} openByDefault={true}>
      <table className="nested-list-body">
        <thead>
          <tr>
            <th>#</th>
            <th>Nazwa</th>
            <th>Waga</th>
          </tr>
        </thead>
        <tbody>
          {spiceMix.ingredients.map((ingredient, index) => (
            <NestedSpiceMixListItem
              key={ingredient.ingredientId}
              ingredient={ingredient}
              index={index}
            />
          ))}
        </tbody>
      </table>
    </NestedList>
  )
}

interface SpiceMixListProps {
  spiceMix: SpiceMix
}
