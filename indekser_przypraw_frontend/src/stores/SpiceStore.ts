import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import Drawer from '@/types/Drawer.ts'
import { Spice } from '@/types/Spice.ts'

type SpiceState = {
  drawers: Array<Drawer>
}

export const SpiceStore = createSlice({
  name: 'Spice',
  initialState: { drawers: [] } satisfies SpiceState as SpiceState,
  reducers: {
    setDrawers: (state, action: PayloadAction<Array<Drawer>>) => {
      state.drawers = action.payload
    },
    addSpiceToDrawer: (
      state,
      action: PayloadAction<{ drawerId: number; spice: Spice }>
    ) => {
      const drawerIndex = state.drawers.findIndex(
        (d) => d.drawerId === action.payload.drawerId
      )
      const spiceGroupIndex = state.drawers[drawerIndex].spices.findIndex(
        (sg) => sg.name === action.payload.spice.name
      )
      state.drawers[drawerIndex].spices[spiceGroupIndex].spices.push(
        action.payload.spice
      )
    },
  },
})

export const { setDrawers, addSpiceToDrawer } = SpiceStore.actions
