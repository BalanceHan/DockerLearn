using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using MyCoreDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreBLL
{
    public class OrmHelp
    {
        private readonly DataContext _context;

        public OrmHelp(DataContext context)
        {
            _context = context;
        }

        public class ReturnType<T>
        {
            public int ErrCode { get; set; }

            public string ErrMsg { get; set; }

            public List<T> Data { get; set; }
        }

        public class ReturnTypeStr<T>
        {
            public int ErrCode { get; set; }

            public string ErrMsg { get; set; }

            public string Data { get; set; }
        }

        /// <summary>
        /// 查询(返回所有数据按ID降序排序)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public ReturnType<T> SelectAllDescID<T>(Expression<Func<T, int>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = _context.Set<T>().AsNoTracking().OrderByDescending(express).ToList();
            if (item.FirstOrDefault() != null)
            {
                Msg.ErrCode = 0;
                Msg.ErrMsg = "success";
                Msg.Data = item;
                return Msg;
            }
            Msg.ErrCode = 10001;
            Msg.ErrMsg = "暂无数据";
            return Msg;
        }

        /// <summary>
        /// 查询(返回所有数据按时间降序排序)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public ReturnType<T> SelectAllDescDate<T>(Expression<Func<T, DateTime?>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = _context.Set<T>().AsNoTracking().OrderByDescending(express).ToList();
            if (item.FirstOrDefault() != null)
            {
                Msg.ErrCode = 0;
                Msg.ErrMsg = "success";
                Msg.Data = item;
                return Msg;
            }
            Msg.ErrCode = 10001;
            Msg.ErrMsg = "暂无数据";
            return Msg;
        }

        /// <summary>
        /// 查询(返回第一条数据)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public async Task<T> SelectFirstAsync<T>(Expression<Func<T, bool>> express) where T : class
        {
            var item = await _context.Set<T>().FirstOrDefaultAsync(express);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// 查询(主键查询)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<T> SelectIdAsync<T>(dynamic id) where T : class
        {
            var item = await _context.Set<T>().FindAsync(id);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// 查询(List类型返回条件数据)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public List<T> SelectList<T>(Expression<Func<T, bool>> express) where T : class
        {
            var item = _context.Set<T>().Where(express).ToList();
            if (item.FirstOrDefault() != null)
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// 查询(List类型返回所有数据)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        public List<T> SelectAllList<T>() where T : class
        {
            var item = _context.Set<T>().ToList();
            if (item.FirstOrDefault() != null)
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// 查询(where语句条件查询)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public ReturnType<T> SelectCondition<T>(Expression<Func<T, bool>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = _context.Set<T>().AsNoTracking().Where(express).ToList();
            if (item.FirstOrDefault() != null)
            {
                Msg.ErrCode = 0;
                Msg.ErrMsg = "success";
                Msg.Data = item;
                return Msg;
            }
            Msg.ErrCode = 10001;
            Msg.ErrMsg = "暂无数据";
            return Msg;
        }

        /// <summary>
        /// 查询(where语句条件查询并按时间排序)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <param name="desc">表达式</param>
        /// <returns></returns>
        public ReturnType<T> SelectConditionDesc<T>(Expression<Func<T, bool>> express, Expression<Func<T, DateTime?>> desc) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = _context.Set<T>().AsNoTracking().Where(express).OrderByDescending(desc).ToList();
            if (item.FirstOrDefault() != null)
            {
                Msg.ErrCode = 0;
                Msg.ErrMsg = "success";
                Msg.Data = item;
                return Msg;
            }
            Msg.ErrCode = 10001;
            Msg.ErrMsg = "暂无数据";
            return Msg;
        }

        /// <summary>
        /// 数据删除(根据主键删除)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> DeleteIdAsync<T>(dynamic id) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = await _context.Set<T>().FindAsync(id);
            if (item == null)
            {
                Msg.ErrCode = 10001;
                Msg.ErrMsg = "未查询到数据，无法删除！";
                return Msg;
            }
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            return Msg;
        }

        /// <summary>
        /// 数据删除(条件删除)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> DeleteConditionAsync<T>(Expression<Func<T, bool>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = await _context.Set<T>().FirstOrDefaultAsync(express);
            if (item == null)
            {
                Msg.ErrCode = 10001;
                Msg.ErrMsg = "未查询到数据，无法删除！";
                return Msg;
            }
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            return Msg;
        }

        /// <summary>
        /// 数据删除(删除所有符合条件数据)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> DeleteAllAsync<T>(Expression<Func<T, bool>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = _context.Set<T>().Where(express);
            foreach (var info in item)
            {
                _context.Set<T>().Remove(info);
            }
            await _context.SaveChangesAsync();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            return Msg;
        }

        /// <summary>
        /// 数据添加(无数据返回添加)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">数据集合</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> InsertDateAsync<T>(T value) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            await _context.Set<T>().AddAsync(value);
            await _context.SaveChangesAsync();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            return Msg;
        }

        /// <summary>
        /// 数据添加(所有数据返回)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">数据集合</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> InsertReturnAsync<T>(T value) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            await _context.Set<T>().AddAsync(value);
            await _context.SaveChangesAsync();

            var info = _context.Set<T>().ToList();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            Msg.Data = info;
            return Msg;
        }

        /// <summary>
        /// 数据添加(指定条件数据返回)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">数据集合</param>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> InsertConditionAsync<T>(T value, Expression<Func<T, bool>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            await _context.Set<T>().AddAsync(value);
            await _context.SaveChangesAsync();

            var info = _context.Set<T>().Where(express).ToList();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            Msg.Data = info;
            return Msg;
        }

        /// <summary>
        /// 数据添加(返回新增数据)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">数据集合</param>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public async Task<T> InsertFirstAsync<T>(T value, Expression<Func<T, bool>> express) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            await _context.Set<T>().AddAsync(value);
            await _context.SaveChangesAsync();

            var info = await _context.Set<T>().FirstOrDefaultAsync(express);
            return info;
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">数据集合</param>
        /// <param name="express">表达式</param>
        /// <returns></returns>
        public async Task<ReturnType<T>> UpdateAsync<T>(Expression<Func<T, bool>> express, Delta<T> data) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            var item = await _context.Set<T>().FirstOrDefaultAsync(express);
            data.Patch(item);

            await _context.SaveChangesAsync();
            var info = _context.Set<T>().AsNoTracking().Where(express).ToList();
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            Msg.Data = info;
            return Msg;
        }

        /// <summary>
        /// 数据返回类型封装
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value">数据集合</param>
        /// <returns></returns>
        public ReturnType<T> GetFormat<T>(List<T> value) where T : class
        {
            ReturnType<T> Msg = new ReturnType<T>();
            if (value.FirstOrDefault() == null)
            {
                Msg.ErrCode = 10001;
                Msg.ErrMsg = "暂无数据";
                return Msg;
            }
            Msg.ErrCode = 0;
            Msg.ErrMsg = "success";
            Msg.Data = value;
            return Msg;
        }

        /// <summary>
        /// 状态返回
        /// </summary>
        /// <param name="errcode">返回状态(数字编号)</param>
        /// <param name="errmsg">返回说明文字</param>
        /// <returns></returns>
        public static ReturnType<string> GetReturn(int errcode, string errmsg)
        {
            ReturnType<string> Msg = new ReturnType<string>
            {
                ErrCode = errcode,
                ErrMsg = errmsg
            };
            return Msg;
        }

        /// <summary>
        /// 数据返回
        /// </summary>
        /// <param name="errcode">返回状态(数字编号)</param>
        /// <param name="errmsg">返回说明文字</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static ReturnTypeStr<string> GetReturn(int errcode, string errmsg, string data)
        {
            ReturnTypeStr<string> Msg = new ReturnTypeStr<string>
            {
                ErrCode = errcode,
                ErrMsg = errmsg,
                Data = data,
            };
            return Msg;
        }
    }
}
