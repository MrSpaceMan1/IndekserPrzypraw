import wretch from 'wretch'

const apiUrl = window.location.hostname

const spiceApi = wretch('http://' + apiUrl + '/api/', {
  credentials: 'include',
})

export default spiceApi
