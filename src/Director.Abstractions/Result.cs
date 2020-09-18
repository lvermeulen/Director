using System;
using System.Collections.Generic;
using System.Linq;

namespace Director.Abstractions
{
	public class Result<T>
	{
		public static Result<T> From(T t)
		{
			return new Result<T>(t);
		}

		public static Result<T> Fail(string error)
		{
			var result = new Result<T> { _errors = new List<string> { error } };
			return result;
		}

		public static Result<T> Fail(IEnumerable<string> errors)
		{
			var result = new Result<T> { _errors = errors.ToList() };
			return result;
		}

		private readonly T _value;
		private List<string> _errors = new List<string>();

		public T Value => IsSuccess ? _value : throw new InvalidOperationException(string.IsNullOrEmpty(Error) ? "The operation was not successful" : Error);
		public string Error => Errors.Any() ? _errors[0] : "";
		public IEnumerable<string> Errors => _errors;

		public bool IsSuccess { get; }

		protected Result()
		{ }

		public Result(T t)
		{
			_value = t;
			IsSuccess = true;
		}

		public Result(IEnumerable<string> errors)
		{
			_errors = errors.ToList();
			IsSuccess = false;
		}
	}
}
