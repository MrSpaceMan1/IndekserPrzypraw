import Ingredient from '@/types/Ingredient.ts'

export default interface SpiceMix {
  name: string
  spiceMixRecipeId: number
  ingredients: Ingredient[]
}
