import React, { Component } from 'react'
import { connect } from 'react-redux'
import {bindActionCreators} from 'redux'
import { selectOutput, deselectOutput } from '../actions/selectActions'
import { selectionReducer } from '../reducers/selectionReducer'
import { patchFixture } from '../actions/stateActions'
import { ItemInfoRow } from '../components/itemInfoRow'
import store from '../store'
import { highlight } from '../actions/actions'

class OutputButton extends Component {

    constructor(props) {
        super(props)
        this.selected = false
        this.htmlId = this.props.data.id + this.props.data.name
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(this.selected != this.isSelected(this.props.data.id, nextProps.selection)) {
            this.selected = !this.selected
            return true
        }
        if(this.props.data != nextProps.data) {
            return true
        }

        return false
    }

    isSelected(id, selection) {
        let ret = false
        selection.outputs.forEach(i => { if(id === i) ret = true})
        return ret
    }



    handleClick() {
        this.props.click.clicked = true
        if(this.props.selection.onlyFixture) {    
            this.props.patchFixture(this.props.selection.fixtures, this.props.data.id)
        }
        const action = this.selected ? this.props.deselectOutput : this.props.selectOutput
        action(this.props.data.id, false)
        
    }

    getPorts(prots) {
        return prots.map((p, i) => {
            const port = p.Net + "." + p.SubNet + "." + p.Universe;
            return(
                <ItemInfoRow name={"Port"+i} value={port} key={"prot" + this.id + i}/>
            )
        })
    }

    render() {
        if(store.getState().hilightStateReducer.on) {
            this.props.highlight();
        }
        return(
            <div
            id={this.htmlId}
            className={
                "button btn btn-outline-primary " + 
                "output-button item-button " + 
                (this.selected ? "active" : "")} 
            area-pressed={ this.selected ? "true" : "false" } 
            autoComplete="off"
            onClick={() => this.handleClick() }
            >
                <div><b>{this.props.data.name}</b></div>
                { this.getPorts(this.props.data.ports) }
                <ItemInfoRow 
                name={"IP"} 
                value={this.props.data.ip} 
                key={this.props.data.ip + this.props.data.id}/>
            </div>
        );

    }
}

const mapStatToProps = (state) => {
    return {
        selection: state.selectionReducer
    }
}

const mapDeispatchToProps = (dispatch) => {
    return bindActionCreators( 
    {
        selectOutput: selectOutput,
        deselectOutput: deselectOutput,
        patchFixture: patchFixture,
        highlight: highlight
    },dispatch)
}

export default connect(mapStatToProps, mapDeispatchToProps)(OutputButton);