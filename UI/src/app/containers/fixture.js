import React, { Component } from 'react';
import { connect } from 'react-redux'
import {bindActionCreators} from 'redux'
import { ItemInfoRow } from '../components/itemInfoRow'
import {
    getShortNameFixtureMode,
    getShortNameFixturePatch,
    getPortString,
    getPatchPixelCount,
    getModePixelCount
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
        this.dmxActive = this.isDmxActive(props)
        this.dmxReducerEventId = -1;
    }

    shouldComponentUpdate(nextProps, nextState) {

        const selected = this.isSelected(this.props.fixture.Id, nextProps)

        if(this.selected != selected) {
            this.selected = !this.selected
            return true
        }

        if(this.props.fixture != nextProps.fixture) {
            return true
        }

        if(selected) {
            return true
        }

        const eid = nextProps.dmxActiveProts.eventId
        if(this.dmxReducerEventId != eid) {
            this.dmxReducerEventId = eid
            let hasDmx = this.isDmxActive(nextProps)
            if(hasDmx != this.dmxActive) {
                this.dmxActive = hasDmx
                return true
            }
        }

        return false
    }

    isDmxActive(props) {
        let ports = props.dmxActiveProts.content
        let port = this.props.fixture.Address.Port
        return ports.findIndex(p => 
            p.Net == port.Net 
            && p.SubNet == port.SubNet 
            && p.Universe == port.Universe) 
            >= 0
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

    renderSelectoinNr() {

        const index = this.props.selection.fixtures.findIndex(f => f == this.props.fixture.Id)

        const style = {
            position: "absolute",
            color: "rgb(127,255,255)"
        }

        return(
            <div style={style}>{this.selected ? index : ""}</div>
        )
    }



    render() {
        const i = this.props.fixture.CurrentModeIndex
        this.mode = getShortNameFixtureMode(this.props.fixture.Modes[i])
        this.patch = getShortNameFixturePatch(this.props.fixture.PixelPatch)
        this.port = getPortString(this.props.fixture.Address.Port)
        this.address = this.props.fixture.Address.DmxAddress
        this.utilAddress = this.props.fixture.UtilAddress.DmxAddress
        
        const cln = "button btn btn-outline-secondary fixture-button item-button " +
                    (this.selected ? "active" : "")

        const dmxStyle = {
            backgroundColor: this.selected ? 'rgba(20,20,20,0.7)' : this.dmxActive ? 'rgba(0,200,0,0.3)' : 'rgba(200,0,0,0.1)'
        }

        return(          
            <div
            style={dmxStyle}
            className={cln}
            id={this.htmlId}
            area-pressed={this.selected ? "true" : "false" }
            onClick={() => this.handleClick() } 
            >
                {this.renderSelectoinNr()}
                <div><b>{this.props.fixture.Name}</b></div>
                <div>
                    <ItemInfoRow name={'Mode'} value={this.mode} />
                    <ItemInfoRow name={'Patch'} value={this.patch} />
                    <ItemInfoRow name={'Address'} value={this.port + "/" + this.address} />
                    <ItemInfoRow name={'Util address'} value={this.utilAddress}/>
                    <ItemInfoRow 
                    name={'Pixel count'} 
                    value={getModePixelCount(this.props.fixture.Modes) + "/" + getPatchPixelCount(this.props.fixture.PixelPatch)} />
                </div>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        selection: state.selectionReducer,
        dmxActiveProts: state.portDmxStateReducer
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