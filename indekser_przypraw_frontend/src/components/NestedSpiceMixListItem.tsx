import { Ingredient } from '@/types'
import './NestedListItem.css'

export default function NestedSpiceMixListItem({
  ingredient,
  index,
}: NestedListItemProps) {
  return (
    <tr key={ingredient.ingredientId}>
      <td>{index + 1}</td>
      <td>{ingredient.name}</td>
      <td>{ingredient.grams}g</td>
    </tr>
  )
}

interface NestedListItemProps {
  ingredient: Ingredient
  index: number
}
