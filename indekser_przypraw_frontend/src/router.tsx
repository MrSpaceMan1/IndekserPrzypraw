import { createBrowserRouter } from 'react-router-dom'
import MainPage from '@/views/MainPage.tsx'
import BarcodeScanner from '@/views/BarcodeScanner.tsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <MainPage />,
  },
  {
    path: '/barcode-scanner',
    element: <BarcodeScanner />,
  },
])
export default router
