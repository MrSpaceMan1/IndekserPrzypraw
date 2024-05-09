import wretch from 'wretch'

// const apiUrl = import.meta.env.VITE_API_HOST ?? window.location.hostname + ':7027'
const apiUrl = window.location.hostname

const spiceApi = wretch('https://' + apiUrl + '/api/', {
  credentials: 'include',
})

export default spiceApi
