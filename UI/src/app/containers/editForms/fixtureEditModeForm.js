import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { fixtureEditModeFormReducer } from '../../reducers/formReducers'
import { closeEditFixtureModeForm } from '../../actions/formActions'
import { editFixtureMode } from '../../actions/stateActions'
import { FixtureMode } from '../../util/util'
import { 
    formModal,
    contentItem,
    buttonItem,
    rowItem
} from './formItems'
import ModeItemList from '../justItems/modeItemList'
import { openModeManagerForm } from '../../actions/formActions'
import { convertModeIndexesToModevalues } from '../../util/mode'
import store from '../../store'

const FORM_ID_MODE = "EDIT_FIXTURE_FORM_ID_MODE"
const FORM_ID_MODE_COLS = "EDIT_FIXTURE_FORM_ID_MODE_COLS"
const FORM_ID_MODE_ROWS = "EDIT_FIXTURE_FORM_ID_MODE_ROWS"


class FixtureEditModeForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
    }

    handleClick() {
        this.props.click.clicked = true
    }

    onClose(){
        this.props.closeEditFixtureModeForm(this)
    }

    onSubmit() {

        let ids = store.getState().selectionReducer.fixtures
        
        let data = {
            modes : convertModeIndexesToModevalues(store.getState().modesReducer.modes)
        }
        
        this.props.editFixtureMode(ids, data)
        this.props.closeEditFixtureModeForm(this)
    }
    
    getValue(id){
        return document.getElementById(id).value
    }

    onModeManagerClick() {
        this.props.openModeManagerForm()
    }

    render() {
        let form = this.props.form
        let modeVals = [1, 3, 9]
        return formModal(
            form.opened,
            'Edit fixture modes',
            'Yes, I can do it',
            'It\'s time to leave',
            this,
            () => contentItem([
                rowItem([
                    <ModeItemList />
                ])
            ])
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.fixtureEditModeFormReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editFixtureMode: editFixtureMode,
        closeEditFixtureModeForm: closeEditFixtureModeForm,
        openModeManagerForm : openModeManagerForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureEditModeForm)