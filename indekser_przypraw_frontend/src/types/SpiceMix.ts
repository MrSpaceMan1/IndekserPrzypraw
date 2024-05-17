import Ingredient from '@/types/Ingredient.ts'

export default interface SpiceMix{
  name: string,
  spiceMixId: number
  ingredients: Ingredient[]
}