import React, { Component } from 'react'
import { connect } from 'react-redux'
import {bindActionCreators} from 'redux'
import { selectOutput, deselectOutput } from '../actions/selectActions'
import { selectionReducer } from '../reducers/selectionReducer'
import { patchFixture } from '../actions/stateActions'
import { ItemInfoRow } from '../components/itemInfoRow'

class OutputButton extends Component {

    constructor(props) {
        super(props)
        this.id = props.data.id
        this.name = props.data.name
        this.ports = props.data.ports
        this.selected = false
        this.htmlId = this.id + this.name
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(this.selected != this.isSelected(this.id, nextProps.selection)) {
            this.selected = !this.selected
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
            this.props.patchFixture(this.props.selection.fixtures, this.id)
        }
        const action = this.selected ? this.props.deselectOutput : this.props.selectOutput
        action(this.id, false)
        
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
                <div><b>{this.name}</b></div>
                { this.getPorts(this.ports) }
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
        patchFixture: patchFixture
    },dispatch)
}

export default connect(mapStatToProps, mapDeispatchToProps)(OutputButton);