interface ApiError {
  type: string
  title: string
  status: number
  detail: string
  errors: { [key: string]: string[] }
}

export default ApiError
