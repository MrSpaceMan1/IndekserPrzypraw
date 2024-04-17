import SpiceGroup from '@/types/SpiceGroup.ts'

interface Drawer {
  drawerId: number
  name: string
  spices: Array<SpiceGroup>
}

export default Drawer
