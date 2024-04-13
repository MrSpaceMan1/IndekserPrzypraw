import { createBrowserRouter } from 'react-router-dom'
import MainPage from '@/views/MainPage.tsx'
import BarcodeScanner from '@/views/BarcodeScanner.tsx'
import AddSpiceFormPage from '@/views/AddSpiceFormPage.tsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <MainPage />,
  },
  {
    path: '/barcode-scanner',
    element: <BarcodeScanner />,
  },
  { path: '/add-spice', element: <AddSpiceFormPage /> },
])
export default router
