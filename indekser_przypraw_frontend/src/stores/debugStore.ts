import { createSlice, PayloadAction } from '@reduxjs/toolkit'

export const debugStore = createSlice({
  name: 'debug',
  initialState: {
    debugMessage: '',
  },
  reducers: {
    setDebugMessage: (state, action: PayloadAction<string>) => {
      state.debugMessage = action.payload
    },
  },
})

export const { setDebugMessage } = debugStore.actions
