import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { fixtureEditModeFormReducer } from '../../reducers/formReducers'
import { closeEditFixtureModeForm } from '../../actions/formActions'
import { editFixtureMode } from '../../actions/stateActions'
import { FixtureMode } from '../../util/util'
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

    createSelectItem(title, id, values) {

        return (
            <div 
            className="col-auto my-1" 
            key={id}
            onClick={() => this.handleClick() }
            >
            <label className="mr-sm-2" htmlFor="inlineFormCustomSelect">{title}</label>
            <select className="custom-select mr-sm-2" id={id}>
                {values.map((val, i) => {
                    return i === 0 ? 
                    <option value={val} key={title + val} defaultValue>{val}</option> : 
                    <option value={val} key={title + val}>{val}</option>
                    })
                }
            </select>
            </div>
        )
    }
    
    createRowItem(child) {
        return (
            <div className="col-12">
                <div className="form-row align-items-center">
                    {child.map((x) => {return x})}
                </div>    
            </div>
        )
    }

    fillIncrementArray(size, start) {
        let vals = Array.apply(null, Array(size))
        return vals.map((x, i) => {return i + start})
    }

    onClose(){
        this.props.closeEditFixtureModeForm(this)
    }

    onSubmit() {

        let ids = store.getState().selectionReducer.fixtures
        
        let data = {
            mode : this.getValue(FORM_ID_MODE),
            modeCol: this.getValue(FORM_ID_MODE_COLS),
            modeRow : this.getValue(FORM_ID_MODE_ROWS)
        }
        
        this.props.editFixtureMode(ids, data)
        this.props.closeEditFixtureModeForm(this)
        
    }



    getValue(id){
        return document.getElementById(id).value
    }

    render() {
        let form = this.props.form
        let modeVals = [1, 3, 9]
        return(
            <Modal visible={form.opened} onClickBackdrop={() => this.onClose() }>
                <div className="modal-header">
                <h5 className="modal-title">Edit fixture mode</h5>
                </div>
                <div className="modal-body">
                <form>
                <div className="form-row align-items-center">
                    {this.createRowItem([
                        this.createSelectItem("Mode", FORM_ID_MODE, FixtureMode.all())
                    ])}
                    {this.createRowItem([
                        this.createSelectItem("Mode Columns", FORM_ID_MODE_COLS, modeVals),
                        this.createSelectItem("Mode Rows", FORM_ID_MODE_ROWS, modeVals)
                    ])}
                </div>
                </form>
                </div>
                <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => this.onClose() }>
                    Klancel
                </button>
                <button type="button" className="btn btn-primary" onClick={ () => this.onSubmit() }>
                    Edit
                </button>
                </div>
            </Modal>
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
        closeEditFixtureModeForm: closeEditFixtureModeForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureEditModeForm)