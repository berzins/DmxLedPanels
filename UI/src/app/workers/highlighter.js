import React, { Component } from 'react';
import { connect } from 'react-redux'
import {bindActionCreators} from 'redux'
import { highlight } from '../actions/actions'
import { selectionReducer } from '../reducers/selectionReducer'
import store from '../store'


// Todo: probably make this as not an UI object. :)
class Highlighter extends Component {

    render() {
        console.log("hilighter on render")
        if(store.getState().hilightStateReducer.on) {
            this.props.highlight();
        }

        return(<div></div>);
    }
}

const mapStateToProps = (state) => {
    return {
        selection: state.selectionReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        highlight: highlight        
    },dispatch)
}


export default connect(mapStateToProps, mapDispatchToProps)(Highlighter);