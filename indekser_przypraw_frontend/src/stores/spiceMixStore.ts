import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { SpiceMix } from '@/types'

type SpiceMixState = {
  spiceMixes: SpiceMix[]
}

export const spiceMixStore = createSlice({
  name: 'spiceMixStore',
  initialState: {
    spiceMixes: [],
  } satisfies SpiceMixState as SpiceMixState,
  reducers: {
    setSpiceMixes: (state, action: PayloadAction<SpiceMix[]>) => {
      state.spiceMixes = action.payload
    },
    addSpiceMix: (state, action: PayloadAction<SpiceMix>) => {
      state.spiceMixes.unshift(action.payload)
    },
    removeSpiceMix: (state, action: PayloadAction<number>) => {
      const index = state.spiceMixes.findIndex(
        (mix) => mix.spiceMixRecipeId == action.payload
      )
      if (index < 0) return
      state.spiceMixes.splice(index, 1)
    },
  },
})

export const { setSpiceMixes, addSpiceMix, removeSpiceMix } =
  spiceMixStore.actions
