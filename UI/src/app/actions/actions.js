import store from '../store'
import { requestServer } from './stateActions'
import { API_URL } from '../constants/const'


export const SOMETHING_HIGHLIGHTED = "SOMETHING_HIGHLIGHTED"

export const highlight = () => {

    console.log(store.getState())
    const state = store.getState().selectionReducer
    const fixIds = state.fixtures
    const outIds = state.outputs

    let fids = fixIds.join('|')
    let oids = outIds.join('|')
    let url = API_URL + '/highlight/?' + 
    'fixture_id=' + fids +
    '&output_id=' + oids

    return (dispatch) => {
        requestServer(url, dispatch, [SOMETHING_HIGHLIGHTED])
    }
}