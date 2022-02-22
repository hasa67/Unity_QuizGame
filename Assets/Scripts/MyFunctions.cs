using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyFunctions {

	public static void ShuffleAnswers(AnswerData[] array){
		if (array.Length == 2) {
			var tmp1 = array [0];
			var tmp2 = array [1];
			var r = Random.Range (0, 2);
			if (r == 0) {
				return;
			} else {
				array [0] = tmp2;
				array [1] = tmp1;
			}
		} else {
			for (int i = array.Length - 1; i > 0; i--) {
				var r = Random.Range (0, i);
				var tmp = array [i];
				array [i] = array [r];
				array [r] = tmp;
			}
		}
	}

	public static void ShuffleQuestionsArray(QuestionData[] array){
		if (array.Length == 2) {
			var tmp1 = array [0];
			var tmp2 = array [1];
			var r = Random.Range (0, 2);
			if (r == 0) {
				return;
			} else {
				array [0] = tmp2;
				array [1] = tmp1;
			}
		} else {
			for (int i = array.Length - 1; i > 0; i--) {
				var r = Random.Range (0, i);
				var tmp = array [i];
				array [i] = array [r];
				array [r] = tmp;
			}
		}
	}

	public static void ShuffleQuestionsList(List<QuestionData> list){
		if (list.Count == 2) {
			var tmp1 = list [0];
			var tmp2 = list [1];
			var r = Random.Range (0, 2);
			if (r == 0) {
				return;
			} else {
				list [0] = tmp2;
				list [1] = tmp1;
			}
		} else {
			for (int i = list.Count - 1; i > 0; i--) {
				var r = Random.Range (0, i);
				var tmp = list [i];
				list [i] = list [r];
				list [r] = tmp;
			}
		}
	}

	public static void ShuffleQuestionsList2(List<QuestionData> list1, List<int> list2){
		if (list1.Count == 2) {
			var tmp1 = list1 [0];
			var tmp2 = list1 [1];
			var tmp3 = list2 [0];
			var tmp4 = list2 [1];
			var r = Random.Range (0, 2);
			if (r == 0) {
				return;
			} else {
				list1 [0] = tmp2;
				list1 [1] = tmp1;
				list2 [0] = tmp4;
				list2 [1] = tmp3;
			}
		} else {
			for (int i = list1.Count - 1; i > 0; i--) {
				var r = Random.Range (0, i);
				var tmpA = list1 [i];
				list1 [i] = list1 [r];
				list1 [r] = tmpA;
				var tmpB = list2 [i];
				list2 [i] = list2 [r];
				list2 [r] = tmpB;
			}
		}
	}
}
