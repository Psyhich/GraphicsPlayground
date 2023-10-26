import { Compartment } from '@codemirror/state'
import {keymap} from "@codemirror/view"
import {EditorView, basicSetup} from "codemirror"
import {javascript} from "@codemirror/lang-javascript"
import {indentWithTab} from "@codemirror/commands"

import {tutorial1} from "./tutorial1.js";

let editor = new EditorView({
  extensions: [basicSetup, javascript(), keymap.of([indentWithTab]),],
  parent: document.getElementById('editor')
})

editor.dispatch({
  changes: {from: 0, insert: "\n\n\n\n\n\n\n"}
})

tutorial1.start();