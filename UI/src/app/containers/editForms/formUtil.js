// ----

export const fillIncrementArray = (size, start) => {
    let vals = Array.apply(null, Array(size))
    return vals.map((x, i) => {return i + start})
}

// ----

const validInit = {
    valid: false,
    error: null
}

// ----

import { isInteger } from '../../util/util'

export const checkIntegerValue = (start, end, name, value) => {
    if(!isInteger(value)) {
        return{...validInit, valid: false, error: name + " value is not an integer"}
    }
    if(value > end || value < start) {
        return {...validInit, valid: false, error: name + " value should be in a range (1 ... 512)"}
    }
    return {...validInit, valid: true, error: null};
}

// ----

export const checkIpAddress = (ip) => {
    if (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(ip)) {  
        return {...validInit, valid: true, error: null }  
    }
    return {...validInit, valid: false, error: "IP address is not valid"}
}

// ---- 

export const validate = (validatons) => {
    let res = "";
    validatons.map((v) => { if(!v.valid) res = res + v.error + " ||| " })
    return res;
}

// ----