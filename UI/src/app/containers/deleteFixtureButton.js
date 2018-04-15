import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { deleteFixture } from '../actions/stateActions'
import { deselectAll } from '../actions/selectActions'
import { selectionReducer } from '../reducers/selectionReducer'


class DeleteFixtureButton extends Component {

    constructor(props) {
        super(props)
        this.enabled = this.shouldBeEnabled(this.props)
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(this.enabled != this.shouldBeEnabled(nextProps)) {
            this.enabled = !this.enabled
            return true
        }
        return false
    }

    shouldBeEnabled(props) {
        return props.selection.hasFixture ? true : false
    }

    handleClick() {
        this.props.click.clicked = true
        this.props.deleteFixture(this.props.selection.fixtures)
        this.props.deselectAll()
    }

    render() {
        return(
            <div
            className={"button btn btn-danger item-button " + (!this.enabled ? 'disabled' : '')} 
            autoComplete="off"
            onClick={() => this.handleClick()}
            >
                <div><b>Delete Fixture</b></div>
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
    return bindActionCreators({
        deleteFixture: deleteFixture,
        deselectAll: deselectAll
    }, dispatch)
}

export default connect(mapStateToProps,mapDispatchToProps)(DeleteFixtureButton);