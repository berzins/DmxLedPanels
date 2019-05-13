import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { fixtureEditPatchFormReducer } from '../../reducers/formReducers'
import { closeEditFixturePatchForm } from '../../actions/formActions'
import {  editFixturePatch } from '../../actions/stateActions'
import {  FixturePatch } from '../../util/util'
import store from '../../store'
import {
    PATCH_COLUMN_VALUES,
    PATCH_ROW_VALUES
} from '../../constants/const'

const FOMR_ID_PATCH = "EDIT_FIXTURE_FOMR_ID_PATCH"
const FORM_ID_PATCH_COLS = "EDIT_FIXTURE_FORM_ID_PATCH_COLS"
const FORM_ID_PATCH_ROWS = "EDIT_FIXTURE_FORM_ID_PATCH_ROWS"


class FixtureEditPatchFrom extends Component {

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
        this.props.closeEditFixturePatchForm(this)
    }

    onSubmit() {

     
        let ids = store.getState().selectionReducer.fixtures
        
        let data = {
            patch : this.getValue(FOMR_ID_PATCH),
            patchCol : this.getValue(FORM_ID_PATCH_COLS),
            patchRow : this.getValue(FORM_ID_PATCH_ROWS)
        }
        
        this.props.editFixturePatch(ids, data)
        this.props.closeEditFixturePatchForm(this)
    }



    getValue(id){
        return document.getElementById(id).value
    }


    render() {
        let form = this.props.form
        // let patchVals = this.fillIncrementArray(30, 1)
        return(
            <Modal visible={form.opened} onClickBackdrop={() => this.onClose() }>
                <div className="modal-header">
                <h5 className="modal-title">Edit fixture pixel patch</h5>
                </div>
                <div className="modal-body">
                <div className="badge badge-warning" style={{width:100+'%'}}>
                    Warning: Output fixtures will not be affected. 
                 </div>
                <form>
                <div className="form-row align-items-center">

                    {this.createRowItem([
                        this.createSelectItem("Patch type", FOMR_ID_PATCH, FixturePatch.all())
                    ])} 
                    {this.createRowItem([
                        this.createSelectItem("Patch columns", FORM_ID_PATCH_COLS, PATCH_COLUMN_VALUES),
                        this.createSelectItem("Patch rows", FORM_ID_PATCH_ROWS, PATCH_ROW_VALUES)
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
        form: state.fixtureEditPatchFormReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editFixturePatch: editFixturePatch,
        closeEditFixturePatchForm: closeEditFixturePatchForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureEditPatchFrom)