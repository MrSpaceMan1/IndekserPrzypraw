import Spice from '@/types/Spice.ts'

interface SpiceGroup {
  name: string
  minimumGrams: number | null
  minimumCount: number | null
  spiceGroupId: number
  spices: Array<Spice>
}

export default SpiceGroup
