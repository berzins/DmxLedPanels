const UTIL_SUCCESS = "UTIL_SUCCESS"
const UTIL_SERVER_ERROR = "UTIL_ERROR"
const CONNECTION_ERROR = "CONNECTION_ERROR"


const requestServer = (url, dispatch) => {
    axios.get(url)
    .then((response) => {
        if(response.status == 200) {
            dispatch({type: UTIL_SUCCESS, payload: response.data})
        } else {
            dispatch({type: UTIL_SERVER_ERROR, payload: response.data})
        }
    })
    .catch((error) => {
        dispatch({type: CONNECTION_ERROR, payload: error})
    })
}