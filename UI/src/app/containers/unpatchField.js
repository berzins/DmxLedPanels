import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { unpatchFixture } from '../actions/stateActions'
import { selectionReducer } from '../reducers/selectionReducer'
import { stateReducer } from '../reducers/stateReducer';

class UnpatchField extends Component {

    isSelected(id) {
        let ret = false
        this.props.selection.fixtures.forEach(i => { if(id === i) ret = true})
        return ret
    }

    handleClick() {
        let ids = []
        this.props.fixtures
        .forEach(f => {
            if(f.PatchedTo !== -1 && this.isSelected(f.Id)) 
            ids.push(f.Id)
        })
        this.props.unpatchFixture(ids)
    }

    render() {
        return(
            <div
            className={"button btn btn-dark item-button unpatch-field"}
            autoComplete="off"
            onClick={ () => this.handleClick() }
            >
                <div><b>U</b></div>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        selection: state.selectionReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators(
        {
            unpatchFixture: unpatchFixture
        }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(UnpatchField);