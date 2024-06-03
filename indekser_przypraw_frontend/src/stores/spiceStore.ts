import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import Drawer from '@/types/Drawer.ts'
import { Spice } from '@/types'
import spiceGroup from '@/types/SpiceGroup.ts'

type SpiceState = {
  drawers: Array<Drawer>
  selectedDrawer: number
}

export const SpiceStore = createSlice({
  name: 'Spice',
  initialState: {
    drawers: [],
    selectedDrawer: 0,
  } satisfies SpiceState as SpiceState,
  reducers: {
    setDrawers: (state, action: PayloadAction<Array<Drawer>>) => {
      state.drawers = action.payload
    },
    addDrawer: (state, action: PayloadAction<Drawer>) => {
      state.drawers.push(action.payload)
    },
    removeDrawer: (state, action: PayloadAction<Drawer>) => {
      state.drawers.splice(
        state.drawers.findIndex((d) => d.drawerId == action.payload.drawerId),
        1
      )
    },
    editDrawer: (
      state,
      action: PayloadAction<Partial<Drawer> & { drawerId: number }>
    ) => {
      const index = state.drawers.findIndex(
        (drawer) => drawer.drawerId === action.payload.drawerId
      )
      if (index >= 0)
        state.drawers[index] = { ...state.drawers[index], ...action.payload }
    },
    addSpiceToDrawer: (
      state,
      action: PayloadAction<{ drawerId: number; spice: Spice }>
    ) => {
      const drawerIndex = state.drawers.findIndex(
        (d) => d.drawerId === action.payload.drawerId
      )
      const spiceGroupIndex = state.drawers[drawerIndex].spices.findIndex(
        (sg) => sg.spiceGroupId === action.payload.spice.spiceGroupId
      )
      if (spiceGroupIndex === -1)
        state.drawers[drawerIndex].spices.push({
          spiceGroupId: action.payload.spice.spiceGroupId,
          name: action.payload.spice.name,
          spices: [action.payload.spice],
          minimumCount: null,
          minimumGrams: null,
        })
      else state.drawers[drawerIndex].spices[spiceGroupIndex].spices
    },
    removeSpiceFromDrawer: (
      state,
      action: PayloadAction<{ drawerId: number; spice: Spice }>
    ) => {
      const drawerIndex = state.drawers.findIndex(
        (d) => d.drawerId === action.payload.drawerId
      )
      const spiceGroupIndex = state.drawers[drawerIndex].spices.findIndex(
        (sg) => sg.name === action.payload.spice.name
      )
      const spiceIndex = state.drawers[drawerIndex].spices[
        spiceGroupIndex
      ].spices.findIndex(
        (spice) => spice.spiceId === action.payload.spice.spiceId
      )
      state.drawers[drawerIndex].spices[spiceGroupIndex].spices.splice(
        spiceIndex,
        1
      )
    },
    removeSpiceGroupFromDrawer: (
      state,
      action: PayloadAction<{ drawerId: number; spiceGroupId: number }>
    ) => {
      const drawerIndex = state.drawers.findIndex(
        (drawer) => drawer.drawerId === action.payload.drawerId
      )
      const spiceGroupIndex = state.drawers[drawerIndex].spices.findIndex(
        (spiceGroup) => spiceGroup.spiceGroupId === action.payload.spiceGroupId
      )
      if (spiceGroupIndex >= 0)
        state.drawers[drawerIndex].spices.splice(spiceGroupIndex, 1)
    },
    updateSpiceGroupFromDrawer: (
      state,
      action: PayloadAction<{ drawerId: number; spiceGroup: spiceGroup }>
    ) => {
      const drawerIndex = state.drawers.findIndex(
        (drawer) => drawer.drawerId === action.payload.drawerId
      )
      const spiceGroupIndex = state.drawers[drawerIndex].spices.findIndex(
        (spiceGroup) =>
          spiceGroup.spiceGroupId === action.payload.spiceGroup.spiceGroupId
      )
      if (spiceGroupIndex >= 0)
        state.drawers[drawerIndex].spices[spiceGroupIndex] = {
          ...state.drawers[drawerIndex].spices[spiceGroupIndex],
          ...action.payload.spiceGroup,
        }
    },
    setSelected: (state, action: PayloadAction<number>) => {
      state.selectedDrawer = action.payload
    },
  },
})

export const {
  setDrawers,
  addDrawer,
  addSpiceToDrawer,
  removeSpiceFromDrawer,
  editDrawer,
  removeSpiceGroupFromDrawer,
  updateSpiceGroupFromDrawer,
  setSelected,
  removeDrawer,
} = SpiceStore.actions
