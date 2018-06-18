import React, { Component } from 'react';
import { connect } from 'react-redux'
import {bindActionCreators} from 'redux'
import { ItemInfoRow } from '../components/itemInfoRow'
import {
    getShortNameFixtureMode,
    getShortNameFixturePatch,
    getPortString
} from '../util/util'
import { selectFixture, deselectFixture } from '../actions/selectActions'
import { highlight } from '../actions/actions'
import { selectionReducer } from '../reducers/selectionReducer'
import store from '../store'



class Fixture extends Component {

    constructor(props) {
        super(props)
        this.selected = this.isSelected(this.props.fixture.Id, props)
        this.htmlId = this.props.fixture.Id + this.props.fixture.Name
    }

    shouldComponentUpdate(nextProps, nextState) {

        if(this.selected != this.isSelected(this.props.fixture.Id, nextProps)) {
            this.selected = !this.selected
            return true
        }

        if(this.props.fixture != nextProps.fixture) {
            return true
        }
        
        return false
    }

    isSelected(id, nextProps) {      
        let ret = false
        nextProps.selection.fixtures.forEach(i => { if(id === i) ret = true})
        return ret
    }

    handleClick() {
        this.props.click.clicked = true
        const action = this.selected ? this.props.deselectFixture : this.props.selectFixture
        action(this.props.fixture.Id, false)
    }

    render() {

        if(store.getState().hilightStateReducer.on) {
            this.props.highlight();
        }
        const i = this.props.fixture.CurrentModeIndex
        this.mode = getShortNameFixtureMode(this.props.fixture.Modes[i])
        this.patch = getShortNameFixturePatch(this.props.fixture.PixelPatch)
        this.port = getPortString(this.props.fixture.Address.Port)
        this.address = this.props.fixture.Address.DmxAddress
        this.utilAddress = this.props.fixture.UtilAddress.DmxAddress
        
        const cln = "button btn btn-outline-secondary fixture-button item-button " +
                    (this.selected ? "active" : "")
        return(          
            <div
            className={cln}
            id={this.htmlId}
            area-pressed={this.selected ? "true" : "false" }
            onClick={() => this.handleClick() } 
            >
                <div><b>{this.props.fixture.Name}</b></div>
                <div>
                    <ItemInfoRow name={'Mode'} value={this.mode} />
                    <ItemInfoRow name={'Patch'} value={this.patch} />
                    <ItemInfoRow name={'Port'} value={this.port} />
                    <ItemInfoRow name={'Address'} value={this.address} />
                    <ItemInfoRow name={'Util address'} value={this.utilAddress}/>
                </div>
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
        selectFixture: selectFixture,
        deselectFixture: deselectFixture,
        highlight: highlight        
    },dispatch)
}


export default connect(mapStateToProps, mapDispatchToProps)(Fixture);